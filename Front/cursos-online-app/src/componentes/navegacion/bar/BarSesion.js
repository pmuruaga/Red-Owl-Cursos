import {
  IconButton,
  Toolbar,
  Typography,
  makeStyles,
  Button,
  Avatar,
  Drawer,
  List,
  ListItem,
  ListItemText
} from "@material-ui/core";
import React, { useState } from "react";
import { withRouter } from "react-router-dom";
import { useStateValue } from "../../../contexto/store";
import FotoUsuarioTemp from "../../../logo.svg";
import { MenuIzquierda } from "./menuIzquierda";
import { MenuDerecha } from "./menuDerecha";

const useStyles = makeStyles((theme) => ({
  seccionDesktop: {
    display: "none",
    [theme.breakpoints.up("md")]: {
      display: "flex",
    },
  },
  seccionMobile: {
    display: "flex",
    [theme.breakpoints.up("md")]: {
      display: "none",
    },
  },
  grow: {
    flexGrow: 1,
  },
  avatarSize: {
    width: 40,
    height: 40,
  },
  list: {
    width: 250,
  },
  listItemText : {
      fontSize : "14px",
      fontWeight: 600,
      paddingLeft: "15px",
      color: "#212121"
  }
}));

const BarSesion = (props) => {
  const classes = useStyles();

  /* Declaro una constante del tipo useStateValue (se importa), este tiene la variable de 
        sesión global sesionUsuario. Esta esta definida en el index.js de los reducers.
        Esto es lo que me va permitir consumir los datos de la variable de sesión. */
  const [{ sesionUsuario }, dispatch] = useStateValue();
  const [abrirMenuIzquierda, setAbrirMenuIzquierda] = useState(false);
  const [abrirMenuDerecha, setAbrirMenuDerecha] = useState(false);

  const cerrarMenuIzquierda = () => {
    setAbrirMenuIzquierda(false);
  }

  const abrirMenuIzquierdaAction = () => {
    setAbrirMenuIzquierda(true);
  }

  const cerrarMenuDerecha = () => {
    setAbrirMenuDerecha(false);
  }

  const salirSesionApp = () => {
    localStorage.removeItem('token_seguridad');
    props.history.push('/auth/login');
  }

  const abrirMenuDerechaAction = () => {
    setAbrirMenuDerecha(true);
  }

  return (
    <React.Fragment>
      <Drawer
        open={abrirMenuIzquierda}
        onClose={cerrarMenuIzquierda}
        anchor="left"
      >
        <div className={classes.list} onKeyDown={cerrarMenuIzquierda} onClick={cerrarMenuIzquierda}>
          <MenuIzquierda classes={classes}></MenuIzquierda>
        </div>
      </Drawer>

      <Drawer
        open={abrirMenuDerecha}
        onClose={cerrarMenuDerecha}
        anchor="right"
      >
        <div 
          role="button" 
          onClick={cerrarMenuDerecha} 
          onKeyDown={cerrarMenuDerecha}
        >
          <MenuDerecha 
            classes={classes} 
            salirSesion={salirSesionApp} 
            usuario={sesionUsuario ? sesionUsuario.usuario : null} 
            />
        </div>
      </Drawer>

      <Toolbar>
        <IconButton color="inherit" onClick={abrirMenuIzquierdaAction}>
          <i className="material-icons">menu</i>
        </IconButton>

        <Typography variant="h6">Cursos Online</Typography>

        <div class={classes.grow}></div>
        <div class={classes.seccionDesktop}>
          <Button color="inherit">Salir</Button>
          <Button color="inherit">
            {sesionUsuario ? sesionUsuario.usuario.nombreCompleto : ""}
          </Button>
          <Avatar src={FotoUsuarioTemp}></Avatar>
        </div>
        <div class={classes.seccionMobile}>
          <IconButton color="inherit" onClick={abrirMenuDerechaAction}>
            <i className="material-icons">more_vert</i>
          </IconButton>
        </div>
      </Toolbar>
    </React.Fragment>
  );
};

export default withRouter(BarSesion);
