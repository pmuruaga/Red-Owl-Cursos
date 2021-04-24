import { IconButton, Toolbar, Typography, makeStyles, Button, Avatar } from '@material-ui/core';
import React from 'react';
import { useStateValue } from '../../../contexto/store';
import FotoUsuarioTemp from "../../../logo.svg";

const useStyles = makeStyles((theme) => ({
    seccionDesktop : {
        display: "none",
        [theme.breakpoints.up("md")] : {
            display: "flex"
        }
    },
    seccionMobile : {
        display: "flex",
        [theme.breakpoints.up("md")] : {
            display: "none"
        }
    },
    grow : {
        flexGrow: 1
    },
    avatarSize : {
        width: 40,
        height: 40
    }
}))

const BarSesion = () => {
    const classes = useStyles();

    /* Declaro una constante del tipo useStateValue (se importa), este tiene la variable de 
        sesión global sesionUsuario. Esta esta definida en el index.js de los reducers.
        Esto es lo que me va permitir consumir los datos de la variable de sesión. */
    const [{sesionUsuario}, dispatch] = useStateValue();

    return (
        <Toolbar>
            <IconButton color="inherit">
                <i className="material-icons">menu</i>
            </IconButton>
            
            <Typography variant="h6">Cursos Online</Typography>

            <div class={classes.grow}></div>
            <div class={classes.seccionDesktop}>
                <Button color="inherit">Salir</Button>
                <Button color="inherit">{sesionUsuario ? sesionUsuario.usuario.nombreCompleto : ""}</Button>
                <Avatar src={FotoUsuarioTemp}></Avatar>
            </div>
            <div class={classes.seccionMobile}>
                <IconButton color="inherit">
                    <i className="material-icons">more_vert</i>
                </IconButton>
            </div>
        </Toolbar>
    );
};

export default BarSesion;