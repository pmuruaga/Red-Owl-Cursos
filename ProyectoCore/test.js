const fetch = require('node-fetch');

async function getNombre(username){
    const url= 'https://api.github.com/users/${username}';
//Antes si la funcion no era async se hacia así:
// fetch(url)
//         .then(result => result.json())
//         .then(json => {
//             console.log(json);
//         });

    const respuesta = await fetch(url);
    const json = await respuesta.json();
    console.log(json);
}

getNombre('pmuruaga');