import { AppShell, Avatar, Button, Group, Image, TooltipFloating } from "@mantine/core";
import { Link, Outlet } from 'react-router-dom';
import { useState, useEffect } from "react";
import GetUserReq from "../lib/user/GetUserReq.ts";
import UserPrivate from "../lib/interfaces.ts";
import loginReq from "../lib/auth/LoginReq.tsx";
import LogoutReq from "../lib/auth/LogoutReq.ts";

export default function DashboardPanel() {
    const [userData, setUserData] = useState<UserPrivate | null>(null);

    useEffect(() => {
        async function fetchData() {
            const user = await GetUserReq();
            setUserData(user);
        }
        fetchData();
    }, []);

    const Logout = async () => {
        await LogoutReq();
        window.location.href = '/';
    }

    if (!userData) {
        return <div>Loading...</div>;
    }

    return (
        <AppShell
            header={{ height: 60 }}
            padding="md"
        >
            <AppShell.Header style={{ alignItems: "stretch" }}>
                <Group h="100%" px="xl" align="center" style={{ width: "100%" }} justify="space-between" wrap={"nowrap"}>
                    <Image src='../../public/whiteLogo.svg' h={50} style={{ alignItems: "center" }} />
                    {
                        userData.userRole === 1 ? (
                            <Group m={"lg"}>
                                <Button variant={"subtle"} component={Link} to="/reservations">Manage Reservations</Button>
                                <Button variant={"subtle"} component={Link} to="/locations">Browse locations</Button>
                            </Group>
                        ) : (
                            <Group m={"lg"}>
                                <Button variant={"subtle"} component={Link} to="/reservations">Your Reservations</Button>
                                <Button variant={"subtle"} component={Link} to="/locations">Browse locations</Button>
                            </Group>
                        )
                    }
                    <Group>
                        <TooltipFloating label={`${userData.name} ${userData.surname}`}>
                            <Avatar size={40} color="blue.8">{`${userData.name[0]}${userData.surname[0]}`}</Avatar>
                        </TooltipFloating>
                        <Button variant={"outline"} onClick={Logout} >Logout</Button>
                    </Group>
                </Group>
            </AppShell.Header>
            <AppShell.Main style={{ padding: 0, margin: 0 }}><Outlet /></AppShell.Main>
        </AppShell>
    );
}
