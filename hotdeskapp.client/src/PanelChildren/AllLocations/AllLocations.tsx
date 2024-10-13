import {useEffect, useState} from "react";
import {Button, Group, Loader, Modal, Text, Title} from "@mantine/core";
import GetAllLocationsReq from "../../lib/location/GetAllLocationsReq.ts";
import LocationPaper from "./LocationPaper.tsx";
import LocationItem from "../../lib/interfaces.ts";
import UserPrivate from "../../lib/interfaces.ts";
import GetUserReq from "../../lib/user/GetUserReq.ts";
import {useDisclosure} from "@mantine/hooks";
import LocationAddModal from "./LocationAddModal.tsx";


export default function AllLocations() {
    const [locations, setLocations] = useState<LocationItem[]>([]);
    const [loading, setLoading] = useState<boolean>(true);
    const [error, setError] = useState<string | null>(null);
    const [userData, setUserData] = useState<UserPrivate | null>(null);
    const [opened, { open, close }] = useDisclosure(false);

    useEffect(() => {
        async function fetchData() {
            const user = await GetUserReq();
            setUserData(user);
        }
        fetchData();
    }, []);

    useEffect(() => {
        const fetchLocations = async () => {
            try {
                const data = await GetAllLocationsReq();
                setLocations(data);
            } finally {
                setLoading(false);
            }
        };

        fetchLocations();
    }, []);

    if (loading) return <Loader/>;
    if (error) return <div>{error}</div>;

    return (
        <div style={{ paddingTop: "4rem", textAlign: "center" }}>
            <Modal opened={opened} onClose={close} title={<Title order={3}>Add location</Title>}>
                <LocationAddModal />
            </Modal>
            <Group h="100%" pb="md" px="xl" align="center" justify="space-between" style={{
                marginTop: "4rem",
                display: "flex",
                flexDirection: "column",
                width: "100%"
            }}>
                <Title order={1} mb="md" style={{ textAlign: "center" }}>
                    All locations
                </Title>
                {userData?.userRole === 1 && (
                    <Button onClick={open} variant="outline">Add new location</Button>
                )}
            </Group>
            <div>
                {locations.length > 0 ? (
                    locations.map((loc) => (
                        <LocationPaper key={loc.id} loc={loc} isAdmin={userData?.userRole === 1} />
                    ))
                ) : (
                    <Text pt="xl">No locations available.</Text>
                )}
            </div>
        </div>
    );

}
