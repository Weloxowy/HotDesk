import {createRoot} from 'react-dom/client'
import './index.css'
import '@mantine/core/styles.css';
import {MantineProvider} from '@mantine/core';
import {BrowserRouter, Route, Routes} from "react-router-dom";
import Landing from "./Landing/Landing.tsx";
import {theme} from "./theme.tsx";
import Auth from "./Auth/Auth.tsx";
import DashboardPanel from "./Panel/Panel.tsx";

createRoot(document.getElementById('root')!).render(
    <MantineProvider theme={theme}>
        <BrowserRouter>
            <Routes>
                <Route path="/" element={<Landing/>}>
                    {" "}
                </Route>
                <Route path="/login" element={<Auth/>}>
                    {" "}
                </Route>
                <Route path="/" element={<DashboardPanel/>}>
                    <Route path="reservations" element={<Auth/>}/>
                    <Route path="locations" element={<Auth/>}/>
                </Route>
            </Routes>
        </BrowserRouter>
    </MantineProvider>
)
