import React, { useEffect, useState } from 'react';
import style from '../Tool/Style';
import {Button, Container, TextField, Grid, Typography} from '@material-ui/core';
import { actualizarUsuario, obtenerUsuarioActual } from '../../actions/UsuarioAction';
import { useStateValue } from '../../contexto/store';
//const [usuario, setUsuario] creo el objeto const que se llama usuario y tiene una función setUsuario, 
//defino allí el json con la estructura de los datos que voy a necesitar
//manipular luego, y enviar al server por ejemplo.

//const ingresarValoresMemoria = e => { Metodo que voy a llamar al ejecutar el onchange, recibe la representación del objeto de la caja de texto.
//e es el contenido de la caja de texto.
//dentro defino una constante que va recibir los atributos de la caja de texto, el atributo name y el atributo value. const {name, value} = e.target;
//
const PerfilUsuario = () => {
    const [{ sesionUsuario}, dispatch] = useStateValue();
    
    const [usuario, setUsuario] = useState({
        nombreCompleto: '',
        email: '',
        password: '',
        confirmarPassword: '',
        username: ''
    })

    const ingresarValoresMemoria = e => {
        const {name, value} = e.target; // Es la representación de la caja de texto. Luego llamo al setUsuario. realizo el cambio necesario.
        setUsuario(anterior => ({
            ...anterior,
            [name]: value            
        })); //mantiene los datos anteriores con el ...anterior y al name le seteo el value de la caja de texto.
    }

    useEffect(() => {
        obtenerUsuarioActual(dispatch).then(response => {
            console.log('esta es la data del objeto response del usuario actual', response);
            setUsuario(response.data);
        });
    }, []) //Con el corchete vacio le indicas que se llame solo una vez.

    const guardarUsuario = e => {
        e.preventDefault();
        actualizarUsuario(usuario).then(response => {
            console.log('se actualizo el usuario', usuario)
            window.localStorage.setItem("token_seguridad", response.data.token);
        })
    }

    return (
        <Container component="main" maxWidth="md" justify="center">
            <div style={style.paper}>
                <Typography component="h1" variant="h5">
                    Perfil de Usuario
                </Typography>
            </div>            
            <form style={style.form}>
                <Grid container spacing={2}>
                    <Grid item xs={12} md={12}>
                        <TextField name="nombreCompleto"  value={usuario.nombreCompleto} onChange={ingresarValoresMemoria} variant="outlined" fullWidth label="Ingrese Nombre y Apellido" ></TextField>
                    </Grid>
                    <Grid item xs={12} md={6}>
                        <TextField name="username" value={usuario.username} onChange={ingresarValoresMemoria} variant="outlined" fullWidth label="Ingrese Username" ></TextField>
                    </Grid>
                    <Grid item xs={12} md={6}>
                        <TextField name="email" value={usuario.email} onChange={ingresarValoresMemoria} variant="outlined" fullWidth label="Ingrese Email" ></TextField>
                    </Grid>
                    <Grid item xs={12} md={6}>
                        <TextField name="password" value={usuario.password} onChange={ingresarValoresMemoria} type="password" variant="outlined" fullWidth label="Ingrese Password" ></TextField>
                    </Grid>
                    <Grid item xs={12} md={6}>
                        <TextField name="confirmarPassword" value={usuario.confirmarPassword} onChange={ingresarValoresMemoria} type="password" variant="outlined" fullWidth label="Confirme Password" ></TextField>
                    </Grid>
                </Grid>
                <Grid container justify="center">
                    <Grid item xs={12} md={6}>
                        <Button type="submit" onClick = {guardarUsuario} fullWidth variant="contained" size="large" color="primary" style={style.submit}>
                            Guardar Datos
                        </Button>
                    </Grid>
                </Grid>
            </form>
        </Container>
    );
};

export default PerfilUsuario;