import {createRoot} from 'react-dom/client'
import './index.css'
import '@mantine/core/styles.css';
import '@mantine/dates/styles.css';
import 'mantine-react-table/styles.css';
import {MantineProvider} from '@mantine/core';
import {BrowserRouter, Route, Routes} from "react-router-dom";
import Landing from "./Landing/Landing.tsx";
import {theme} from "./theme.tsx";
import Auth from "./Auth/Auth.tsx";
import DashboardPanel from "./Panel/Panel.tsx";
import YourReservation from "./PanelChildren/YourReservations/YourReservations.tsx";
import AllLocations from "./PanelChildren/AllLocations/AllLocations.tsx";
import LocationDesks from "./PanelChildren/LocationDesks/LocationDesks.tsx";
import DeskInfo from "./PanelChildren/Desk/DeskInfo.tsx";
import AllReservations from "./PanelChildren/AllReservations/AllReservations.tsx";

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
                    <Route path="reservations" element={<YourReservation/>}/>
                    <Route path="admin-reservations" element={<AllReservations/>}/>
                    <Route path="locations" element={<AllLocations/>}/>
                    <Route path="location/:id" element={<LocationDesks/>}/>
                    <Route path="desk/:id" element={<DeskInfo/>}/>
                </Route>
            </Routes>
        </BrowserRouter>
    </MantineProvider>
)
