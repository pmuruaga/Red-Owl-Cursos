import React, {useState} from 'react';
import { ThemeProvider as MuithemeProvider } from "@material-ui/core/styles";
import theme from "./theme/theme";
import RegistrarUsuario from './componentes/seguridad/RegistrarUsuario';
import Login from './componentes/seguridad/Login';
import PerfilUsuario from './componentes/seguridad/PerfilUsuario';

function App() {
  return (
    // <MuithemeProvider theme={theme}>
    //     {/* <h1>Proyecto en Blanco</h1>
    //     <TextField variant="outlined" />
    //     <Button variant="contained" color="primary">Mi Boton Material Design</Button> */}
    //     <RegistrarUsuario></RegistrarUsuario>
    // </MuithemeProvider>
    <MuithemeProvider theme={theme}>
      <RegistrarUsuario></RegistrarUsuario>
    </MuithemeProvider>
  )
}

export default App;
