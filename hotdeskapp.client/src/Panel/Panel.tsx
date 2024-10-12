import {AppShell, Avatar, Button, Group, Image, TooltipFloating} from "@mantine/core";
import {Link, Outlet} from 'react-router-dom';

export default function DashboardPanel() {
    return (
        <AppShell
            header={{height: 60}}
            padding="md"
        >
            <AppShell.Header style={{alignItems: "stretch"}}>
                <Group h="100%" px="xl" align="center" style={{width: "100%"}} justify="space-between" wrap={"nowrap"}>
                    <Image src='../../public/whiteLogo.svg' h={50} style={{alignItems: "center"}}/>

                    <Group m={"lg"}>
                        <Button variant={"subtle"} component={Link} to="/reservations">Reservations</Button>
                        <Button variant={"subtle"} component={Link} to="/locations">Browse locations</Button>
                    </Group>

                    <Group>
                        <TooltipFloating label={"Artur Wiech"}>
                            <Avatar size={40} color="blue.8">AW</Avatar>
                        </TooltipFloating>
                        <Button variant={"outline"}>Logout</Button>
                    </Group>
                </Group>
            </AppShell.Header>
            <AppShell.Main style={{padding: 0, margin: 0}}><Outlet/></AppShell.Main>
        </AppShell>
    )
}
