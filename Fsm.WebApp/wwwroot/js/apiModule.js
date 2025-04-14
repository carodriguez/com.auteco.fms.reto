// apiModule.js
const apiModule = (function () {

    // Función privada para manejar la respuesta del fetch
    async function handleResponse(response) {

        return await response.json();

    }

    // Función pública para hacer una petición GET a la API
    async function get(endpoint) {

        const response = await fetch(endpoint, {
            method: 'GET',
            headers: {
                'Content-Type': 'application/json',
            },
        });

        return handleResponse(response);
    }

    // Función pública para hacer una petición POST a la API
    async function postFromBody(apiUrl, data) {
        debugger;
        const response = await fetch(apiUrl, {
            method: 'POST',
            headers: {
                'Accept': 'application/json; charset=utf-8',
                'Content-Type': 'application/json;charset=UTF-8'
                // Falta agregar el token de autenticación a la cabecera

            },
            body: JSON.stringify(data),
        });

        return handleResponse(response);
    }



    // Función pública para hacer una petición POST a la API con un archivo adjunto
    async function postFromBodyFile(apiUrl, formData) {
        debugger;
        const response = await fetch(apiUrl, {
            method: 'POST',
            body: formData,
            headers: {
                'enctype': 'multipart/form-data',
            },
        });

        return handleResponse(response);
    }

    // Función pública para hacer una petición POST a la API con parametros de tipo fromquery y frombody
    async function postFromQuery(apiUrl) {

        debugger;
        const response = await fetch(apiUrl, {
            method: 'POST',
            headers: {
                'Content-Type': 'application/x-www-form-urlencoded',

            }

        });

        return handleResponse(response);
    }
      

    // Otras funciones públicas para métodos HTTP adicionales si es necesario
        // Exponer las funciones públicas
    return {
        get: get,
        postFromQuery: postFromQuery,
        postFromBody: postFromBody,
        postFromBodyFile: postFromBodyFile
       // Otros métodos públicos si los hay
    };
})();



