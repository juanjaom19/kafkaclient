using System.Linq.Expressions;
using AutoMapper;
using kafkaclient.web.Core.Dto;
using kafkaclient.web.Core.Entities;
using kafkaclient.web.Core.Interfaces;
using kafkaclient.web.Core.Utils;
using Microsoft.EntityFrameworkCore;

namespace kafkaclient.web.Core.Services;

public class ClusterService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    private readonly Dictionary<string, string> operators = new Dictionary<string, string>()
    {
        {"Name", "like"},
        {"Version", "like"},
        {"Host", "like"},
        {"Path", "like"},

        {"Status" , "="},

        {"CreatedAt", ">="}
    };

    private readonly List<FilterGroup> filters = new List<FilterGroup>{
        new FilterGroup {
            PropertyName = "Search",
            MultipleProperties = new string[]{ "Name", "Version", "Host", "Path" },
            LogicalOperator = "Or",
            Group = 1
        },
        new FilterGroup { PropertyName = "Status", LogicalOperator = "And", Group = 2},
        new FilterGroup { PropertyName = "CreatedAt", LogicalOperator = "And", Group = 2}
    };

    public ClusterService(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<PageResponse<ClusterDto>> GetAllAsync(
        PagingRequest query_paging,
        ClusterFilterRequest query_filter,
        ClusterOrderByRequest? query_order_by,
        bool add_includes = false)
    {
        var result = await _unitOfWork.Clusters.GetAllAsync<ClusterFilterRequest, ClusterOrderByRequest>(
            offset: (int)query_paging.Offset,
            limit: (int)query_paging.Limit,
            filter: query_filter,
            orderBy: query_order_by,
            operators: operators,
            filterGroups: filters
        );

        var productDtos = _mapper.Map<IEnumerable<ClusterDto>>(result.Data);

        var response = new PageResponse<ClusterDto>{
            Total = result.Total,
            Count = result.Count,
            Limit = result.Limit,
            Offset = result.Offset,
            Data = productDtos.ToList(),
        };
        return response;
    }

    public async Task<ClusterDto> GetByIdAsync(int id, bool add_includes = false)
    {
        var result = await _unitOfWork.Clusters.GetByIdAsync(
            id
        );

        if (result == null)
        {
            throw new NotFoundException($"The cluster with id {id} does not exist");
        }

        return _mapper.Map<ClusterDto>(result);
    }

    public async Task<ClusterDto> CreateAsync(ClusterRequest request)
    {
        var cluster = _mapper.Map<Cluster>(request);
        cluster.Slug = SlugGenerator.GenerateSlug(cluster.Name);

        await ValidateName(cluster.Name);
        await ValidateSlug(cluster.Slug);

        var newCluster = await _unitOfWork.Clusters.CreateAsync(cluster);

        if (newCluster.Id == 0)
        {
            throw new UnexpectedException("Can't add object to db");
        }

        return _mapper.Map<ClusterDto>(newCluster);
    }

    public async Task<ClusterDto> UpdateAsync(int id, ClusterRequest request)
    {
        await ValidateName(request.Name, id);
        await ValidateSlug(request.Slug, id);

        var cluster = await _unitOfWork.Clusters.GetByIdAsync(id);
        if (cluster == null)
        {
            throw new NotFoundException($"The cluster with id {id} does not exist");
        }

        _mapper.Map(request, cluster);
        var affectedRows = await _unitOfWork.Clusters.UpdateAsync(cluster);
        if (affectedRows == 0)
        {
            throw new UnexpectedException("Can't add object to db");
        }

        return _mapper.Map<ClusterDto>(cluster);
    }

    public async Task<ClusterDto> UpdateAsync(int id)
    {
        var cluster = await _unitOfWork.Clusters.GetByIdAsync(id);
        if (cluster == null)
        {
            throw new NotFoundException($"The cluster with id {id} does not exist");
        }

        cluster.Status = !cluster.Status;

        var affectedRows = await _unitOfWork.Clusters.UpdateAsync(cluster);
        if (affectedRows == 0)
        {
            throw new UnexpectedException("Can't add object to db");
        }

        return _mapper.Map<ClusterDto>(cluster);
    }

    public async Task<DeleteResponse> DeleteAsync(int id)
    {
        var cluster = await _unitOfWork.Clusters.GetByIdAsync(id);
        if (cluster == null)
        {
            throw new NotFoundException($"The cluster with id {id} does not exist");
        }

        var affectedRows = await _unitOfWork.Clusters.DeleteAsync(cluster);
        return new DeleteResponse{
            GeneralMessage = "Success"
        };
    }

    private async Task ValidateName(string name, int? id = null)
    {
        Expression<Func<Cluster,bool>> queryExpression = (Cluster p) => 
                p.Name.ToLower().Equals(name.ToLower());
        
        if (id != null){
            queryExpression = (Cluster p) => 
                p.Name.ToLower().Equals(name.ToLower()) && p.Id != id;
        }
        
        // Review if the name of the category exist on the database for the country
        var existCluster = await _unitOfWork.Clusters.ExistAnyAsync(queryExpression);
        // if exist return 409
        if (existCluster)
        {
            throw new ConflictException($"Already exists, name: {name}");
        }
    }

    private async Task ValidateSlug(string value, int? id = null)
    {
        Expression<Func<Cluster,bool>> queryExpression = (Cluster p) => 
                p.Slug.ToLower().Equals(value.ToLower());
        
        if (id != null){
            queryExpression = (Cluster p) => 
                p.Slug.ToLower().Equals(value.ToLower()) && p.Id != id;
        }
        
        // Review if the name of the category exist on the database for the country
        var existCluster = await _unitOfWork.Clusters.ExistAnyAsync(queryExpression);
        // if exist return 409
        if (existCluster)
        {
            throw new ConflictException($"Already exists, slug: {value}");
        }
    }
}