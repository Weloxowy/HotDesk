import {AppShell, Button, Flex, Group, Image, rem, Space, Text, Title} from "@mantine/core";
import {Link} from "react-router-dom";

export default function Landing() {
    return (
        <AppShell
            header={{height: 60}}
            padding="md"
        >
            <AppShell.Header style={{alignItems: "stretch"}}>
                <Group h="100%" px="xl" align="center" style={{width: "100%"}} justify="space-between" wrap={"nowrap"}>
                    <Image src='../../public/whiteLogo.svg' h={50} style={{alignItems: "center"}}/>


                    <Group>
                        <Button variant={"outline"} component={Link} to="/login">Login</Button>
                    </Group>
                </Group>
            </AppShell.Header>
            <AppShell.Main>
                <Flex flex={"1 1 auto"} wrap={"wrap"}
                      style={{height: "100vh", justifyContent: "stretch", alignItems: "center"}}>
                    <section style={{width: "50vw", textAlign: "center"}}>
                        <Title size={rem(70)}>Simplify Your Workspace</Title>
                        <Text size={rem(40)}>
                            Effortlessly reserve, manage, and optimize office spaces with our intuitive
                            hotdesk system. Stay flexible and ensure your team always has the perfect spot to
                            collaborate or
                            focus.
                        </Text>
                        <Space h="xl"/>
                        <Button component={Link} to="/login" size={"lg"} fullWidth>Login</Button>
                    </section>
                    <section  style={{width: "40vw", textAlign: "center"}}>
                        <Image src={"../../public/main.jpg"}/>
                    </section>
                </Flex>
            </AppShell.Main>
        </AppShell>

    )
}
