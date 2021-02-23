import axios from 'axios';

//Pongo la url de la api a donde hace las llamadas este cliente.
axios.defaults.baseURL = 'http://localhost:5000/api';

//Defino tambien un interceptor que antes de enviar la llamada le va incluir en el header el token de seguridad.
axios.interceptors.request.use((config) => {
    const token_seguridad = window.localStorage.getItem('token_seguridad');
    if(token_seguridad){
        config.headers.Authorization = 'Bearer ' + token_seguridad;
        return config;
    }
}, error => {
    return Promise.reject(error);
});

const requestGenerico = {
    get: (url) => axios.get(url),
    post: (url, body) => axios.post(url, body),
    put: (url, body) => axios.put(url, body),
    delete: (url) => axios.delete(url)
};

export default requestGenerico;