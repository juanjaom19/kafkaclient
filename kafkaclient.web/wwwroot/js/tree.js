document.addEventListener('DOMContentLoaded', function() {
    const addButton = document.getElementById('addButton');
    const deleteButton = document.getElementById('deleteButton');
    let clickTimer = null;

    const ActionTypes = {
        CLUSTER: 'cluster',
        CLUSTERS: 'clusters',
        BROKERS: 'brokers',
        TOPICS: 'topics',
        CONSUMERS: 'consumers'
    };

    addButton.addEventListener('click', function(event) {
        console.log('Agregar botón clickeado');
        createRipple(event, addButton, 'ripple-button');
        setTimeout(() => {
            window.location.href = createClusterUrl;
        }, 650);
        
    });

    deleteButton.addEventListener('click', function(event) {
        console.log('Eliminar botón clickeado');
        createRipple(event, deleteButton, 'ripple-button');
    });


    document.querySelectorAll('.toggle-icon').forEach(icon => {
        icon.addEventListener('click', (event) => {
            const li = icon.closest('li');
            const nestedList = li.querySelector('.nested');
            if (nestedList) {
                nestedList.classList.toggle('active');
                li.querySelector('.caret').classList.toggle('caret-down');
                const iconElement = icon.querySelector('i');
                if (nestedList.classList.contains('active')) {
                    iconElement.classList.remove('fa-plus');
                    iconElement.classList.add('fa-minus');
                } else {
                    iconElement.classList.remove('fa-minus');
                    iconElement.classList.add('fa-plus');
                }
            }
            saveTreeState()
            event.stopPropagation();
        });
    });


    document.querySelectorAll('#elementsTree li').forEach(li => {
        li.addEventListener('click', (event) => {
            if (clickTimer === null) {
                clickTimer = setTimeout(() => {
                    clickTimer = null;
                    li.querySelector('.caret').classList.add('clicked');
                    selectLiSpan(li);
                    createRipple(event, li.querySelector('.caret'), 'ripple');
                    setTimeout(() => li.querySelector('.caret').classList.remove('clicked'), 500);
                    saveTreeState();
                    window.location.href = listClusterUrl;
                }, 250);
            } else {
                clearTimeout(clickTimer);
                clickTimer = null;
                li.querySelector('.caret').classList.add('double-clicked');
                createRipple(event, li.querySelector('.caret'), 'ripple');
                setTimeout(() => li.querySelector('.caret').classList.remove('double-clicked'), 500);
                const nestedList = li.querySelector('.nested');
                if (nestedList) {
                    nestedList.classList.toggle('active');
                    li.querySelector('.caret').classList.toggle('caret-down');
                    const icon = li.querySelector('.toggle-icon i');
                    if (nestedList.classList.contains('active')) {
                        icon.classList.remove('fa-plus');
                        icon.classList.add('fa-minus');
                    } else {
                        icon.classList.remove('fa-minus');
                        icon.classList.add('fa-plus');
                    }
                }
                saveTreeState()
            }
            event.stopPropagation();
            return;
        });

        li.addEventListener('contextmenu', (event) => {
            event.preventDefault();
            li.querySelector('.caret').classList.add('right-clicked');
            createRipple(event, li.querySelector('.caret'), "ripple");
            setTimeout(() => li.querySelector('.caret').classList.remove('right-clicked'), 500);
            let contextMenuId = "";
            let actionType = getSubstringBeforeFirstHyphenByRegex(li.id);
            switch(actionType) {
                case ActionTypes.CLUSTER:
                    contextMenuId = "contextMenuCluster";
                    break;
                case ActionTypes.CLUSTERS:
                    contextMenuId = "contextMenuClustersConn";
                    break;
                case ActionTypes.BROKERS:
                    break;
                case ActionTypes.TOPICS:
                    break;
                case ActionTypes.TOPICS:
                    break;
                default:
                    break;
            }

            if (contextMenuId) {
                const contextMenu = document.getElementById(contextMenuId);
    
                contextMenu.style.top = `${event.pageY}px`;
                contextMenu.style.left = `${event.pageX}px`;
                contextMenu.style.display = 'block';
    
                document.addEventListener('click', () => {
                    contextMenu.style.display = 'none';
                }, { once: true });
            }
            
        });
    });

    function selectLiSpan(li) {
        document.querySelectorAll('#elementsTree span').forEach(item => item.classList.remove('selected-opt'));
        li.querySelector('.caret').classList.add('selected-opt');
    }

    function saveTreeState() {
        const treeStateNested = [];
        document.querySelectorAll('#elementsTree li').forEach(li => {
            const nestedList = li.querySelector('.nested');
            if (nestedList && nestedList.classList.contains('active')) {
                treeStateNested.push(li.id);
            }
        });
        localStorage.setItem('treeStateNested', JSON.stringify(treeStateNested));

        const selectedLiSpan = document.querySelector('#elementsTree li span.selected-opt');
        if (selectedLiSpan) {
            const selectedLi = selectedLiSpan.parentNode;
            localStorage.setItem('selectedLi', selectedLi.id);
        } else {
            localStorage.removeItem('selectedLi');
        }
    }

    function restoreTreeState() {
        const treeStateNested = JSON.parse(localStorage.getItem('treeStateNested'));
        if (treeStateNested) {
            treeStateNested.forEach(id => {
                const li = document.getElementById(id);
                if (li) {
                    const nestedList = li.querySelector('.nested');
                    if (nestedList) {
                        nestedList.classList.add('active');
                        li.querySelector('.caret').classList.add('caret-down');
                        const icon = li.querySelector('.toggle-icon i');
                        icon.classList.remove('fa-plus');
                        icon.classList.add('fa-minus');
                    }
                }
            });
        }

        const selectedLiId = localStorage.getItem('selectedLi');
        if (selectedLiId) {
            const selectedLi = document.getElementById(selectedLiId);
            if (selectedLi) {
                selectedLi.querySelector('.caret').classList.add('selected-opt');
            }
        }
    }

    restoreTreeState();
});