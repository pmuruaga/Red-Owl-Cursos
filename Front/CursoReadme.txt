-Con el comando npx create-react-app cursos-online-app creo este proyecto react.

-Con npm start desde dentro de la carpeta cursos-online-app corro la app.

En el package.json en laparte de dependencias declaro los paquetes que usare en el proyecto, en este caso react, react-dom, etc...
En scripts tenes los scripts que voy a usar, por ejemplo para iniciar la app: start, etcetc.
En public, en el index.html
<div id="root"></div>
Será donde corran los componentes react.


npm install axios --> libreria para trabajar con webapis.

Para instalar material ui:
npm install @material-ui/core
Luego importo las fuentes, en el head del index.html de public:
<link rel="stylesheet" href="https://fonts.googleapis.com/css?family=Roboto:300,400,500,700&display=swap" />
(Ver web: material-ui.com)
Lo mismo con los iconos:
<link rel="stylesheet" href="https://fonts.googleapis.com/icon?family=Material+Icons" />

Luego llamoa npm install @material-ui/styles

Creo el theme, importo MuithemeProvider en app.js

Luego instalo en la solución el @material-ui/icons para poder usar iconos.


--Instalo Axios, es una libreria que me va permitir manejar la comunicación entre el front y el back, manejando las request.
npm install axios
-Creo la carpeta servicios y el HttpCliente.js donde voy a gestionar las requests. Declaro la variable con la base url.
Creo un request generico con los metodos que voy a usar, sin el then... el then luego lo pondre cuando haga las llamadas.

Luego paso a crear Actions que van a ser funciones globales que haran llamadas usando la libreria axios.

Finalmente uso ese action en el componente haciendo la llamada al action que va a invocar el request de axios.


Instalo react-router-dom, es una libreria que sirve para navegar, para cargar en el app el js que necesite renderizar.
-npm install react-router-dom

Agrego la etiqueta router en el app.js

Podes usar el generador de código de VSCode escribiendo rsc y tab y te crea un archivo con estructura react.

Agrego una carpeta contexto donde voy a crear los archivos para manejar las variables globales de estado de Context API.
No se necesitan instalar otras librerias para trabajar con Context API, directamente viene en React.
En el store defino el stateProvider que es donde voy a definir quinenes van a ser los subscriptos a mi contexto. (Tengo proveedores y consumidores)

El papel de un reducer es acceder a las variables de sesión y cambiar su data.


"Password":"Password12345$",
"Email":"pmuruaga2@gmail.com"

token_seguridad
Bearer