import React, { useEffect, useState } from "react";
import { ThemeProvider as MuithemeProvider } from "@material-ui/core/styles";
import { Grid, Snackbar } from "@material-ui/core";
import theme from "./theme/theme";
import RegistrarUsuario from "./componentes/seguridad/RegistrarUsuario";
import Login from "./componentes/seguridad/Login";
import PerfilUsuario from "./componentes/seguridad/PerfilUsuario";
import { BrowserRouter as Router, Route, Switch } from "react-router-dom";
import AppNavbar from "./componentes/navegacion/AppNavbar";
import { useStateValue } from "./contexto/store";
import { obtenerUsuarioActual } from "./actions/UsuarioAction";

function App() {
  const [{ sesionUsuario, openSnackbar }, dispatch] = useStateValue(); //  Puedo declarar varias variables de estado en el mismo dispatch.

  const [iniciaApp, setIniciaApp] = useState(false);

  useEffect(() => {
    if (!iniciaApp) {
      obtenerUsuarioActual(dispatch)
        .then((response) => {
          setIniciaApp(true);
        })
        .catch((error) => {
          setIniciaApp(true);
        });
    }
  }, [iniciaApp]);

  return (
    // <MuithemeProvider theme={theme}>
    //     {/* <h1>Proyecto en Blanco</h1>
    //     <TextField variant="outlined" />
    //     <Button variant="contained" color="primary">Mi Boton Material Design</Button> */}
    //     <RegistrarUsuario></RegistrarUsuario>
    // </MuithemeProvider>
    <React.Fragment>
      <Snackbar
        anchorOrigin={{ vertical: "bottom", horizontal: "center" }}
        open={openSnackbar ? openSnackbar.open : false}
        autoHideDuration={3000}
        ContentProps={{ "aria-describedby": "message-id" }}
        message={
          <span id="message-id">
            {openSnackbar ? openSnackbar.mensaje : ""}
          </span>
        }
        onClose={() =>
          dispatch({
            type: "OPEN_SNACKBAR",
            openMensaje: {
              open: false,
              mensaje: "",
            },
          })
        }
      ></Snackbar>
      <Router>
        <MuithemeProvider theme={theme}>
          <AppNavbar />
          <Grid container>
            <Switch>
              <Route exact path="/auth/login" component={Login}></Route>
              <Route
                exact
                path="/auth/registro"
                component={RegistrarUsuario}
              ></Route>
              <Route
                exact
                path="/auth/perfil"
                component={PerfilUsuario}
              ></Route>
              <Route exact path="/" component={PerfilUsuario}></Route>
            </Switch>
          </Grid>
        </MuithemeProvider>
      </Router>
    </React.Fragment>
  );
}

export default App;
