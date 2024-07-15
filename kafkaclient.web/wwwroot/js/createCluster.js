document.addEventListener('DOMContentLoaded', () => {
    const saveButton = document.getElementById('saveButton');
    const testConnection = document.getElementById('testConnection');
    const loader = document.getElementById('loader');

    function sleep(ms) {
        return new Promise(resolve => setTimeout(resolve, ms));
    }

    saveButton.addEventListener('click', async (event) => {
        saveButton.disabled = true;
        loader.style.display = 'block';
        createRipple(event, saveButton, 'ripple-button');
        const data = {
            Name: document.getElementById('Name').value,
            Version: document.getElementById('Version').value,
            Host: document.getElementById('Host').value,
            Path: document.getElementById('Path').value
        };

        try {
            const response = await fetch('/api/ClusterApi', {
                method: 'POST',
                headers: {
                    'Content-Type': 'application/json'
                },
                body: JSON.stringify(data)
            });

            const result = await response.json();
            
            await sleep(2000); // Simula una demora de 2 segundos

            if (response.ok) {
                toastr.success(result.message);
                document.getElementById('createClusterForm').reset();
            } else {
                Object.keys(result.errors).forEach(function(key) {
                    console.log(key);
                    if (key === 'Name') document.getElementById('NameError').textContent = result.errors[key].join(', ');
                    if (key === 'Version') document.getElementById('VersionError').textContent = result.errors[key].join(', ');;
                    if (key === 'Host') document.getElementById('HostError').textContent = result.errors[key].join(', ');;
                    if (key === 'Path') document.getElementById('PathError').textContent = result.errors[key].join(', ');;
                });
            }
        } catch (error) {
            toastr.error('A ocurrido un error mientras se creaba el cluster.');
        } finally {
            saveButton.disabled = false;
            loader.style.display = 'none';
        }
    });

    testConnection.addEventListener('click', async (event) => {
        testConnection.disabled = true;
        loader.style.display = 'block';
        createRipple(event, testConnection, 'ripple-button');
        setTimeout(() => {
            testConnection.disabled = false;
            loader.style.display = 'none';
        }, 350);
    });
});