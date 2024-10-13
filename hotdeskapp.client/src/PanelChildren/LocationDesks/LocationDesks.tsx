import {Button, Loader, Modal, Text, Title} from "@mantine/core";
import Desk from "../../lib/interfaces.ts";
import { MantineReactTable, MRT_ColumnDef, MRT_Row } from "mantine-react-table";
import { useEffect, useMemo, useState } from "react";
import GetAllDesksByLocationReq from "../../lib/desk/GetAllDesksByLocationReq.ts";
import { Link, useParams } from "react-router-dom";
import LocationItem from "../../lib/interfaces.ts";
import getLocationByIdReq from "../../lib/location/GetLocationByIdReq.ts";
import UserPrivate from "../../lib/interfaces.ts";
import GetUserReq from "../../lib/user/GetUserReq.ts";
import { IconTrash } from "@tabler/icons-react";
import DeleteDeskReq from "../../lib/desk/DeleteDeskReq.ts";
import { ModalsProvider, modals } from '@mantine/modals';
import {useDisclosure} from "@mantine/hooks";
import DeskAddModal from "../Desk/DeskAddModal.tsx";
export default function LocationDesks() {
    const { id } = useParams();
    const [desks, setDesks] = useState<Desk[]>([]);
    const [location, setLocation] = useState<LocationItem>();
    const [loading, setLoading] = useState<boolean>(true);
    const [error, setError] = useState<string | null>(null);
    const [userData, setUserData] = useState<UserPrivate | null>(null);
    const [opened, { open, close }] = useDisclosure(false);

    useEffect(() => {
        async function fetchData() {
            try {
                const user = await GetUserReq();
                setUserData(user);
            } catch (err) {
                setError("Failed to fetch user data");
            }
        }
        fetchData();
    }, []);

    // Fetch desks and location data
    useEffect(() => {
        const fetchDesks = async () => {
            try {
                const dataLocation = await getLocationByIdReq(id);
                setLocation(dataLocation);
                const dataDesks = await GetAllDesksByLocationReq(id);
                setDesks(dataDesks);

            }finally {
                setLoading(false);
            }
        };

        fetchDesks();
    }, [id]);

    const openDeleteConfirmModal = (row: MRT_Row<Desk>) =>
        modals.openConfirmModal({
            title: 'Are you sure you want to delete this desk?',
            children: (
                <Text>
                    Are you sure you want to delete desk {row.original.name}? It will delete all reservations for this desk. This action cannot be undone.
                </Text>
            ),
            labels: { confirm: 'Delete', cancel: 'Cancel' },
            confirmProps: { color: 'red' },
            onConfirm: async () => {
                try {
                    await DeleteDeskReq(row.original.id);
                    setDesks((prevData) => prevData.filter((desk) => desk.id !== row.original.id));
                } catch {
                    setError('Failed to delete desk');
                }
            },
        });

    const columns = useMemo<MRT_ColumnDef<Desk>[]>(() => [
        {
            accessorKey: "name",
            header: "Name",
        },
        {
            accessorKey: "description",
            header: "Description",
        },
        {
            id: "actions",
            header: "Actions",
            Cell: ({ row }) => (
                <>
                    <Button component={Link} to={`/desk/${row.original.id}`} size="sm">
                        Go to Desk
                    </Button>
                    {userData?.userRole === 1 ? (
                        <Button
                            variant="filled"
                            color="red"
                            leftSection={<IconTrash />}
                            onClick={() => openDeleteConfirmModal(row)}
                            style={{ marginLeft: '10px' }}
                        >
                            Delete
                        </Button>
                    ) : null}
                </>
            ),
        },
    ], [userData]);

    if (loading) return <Loader />;
    if (error) return <div>{error}</div>;

    return (
        <div style={{paddingTop: "4rem",  textAlign: "center"}}>
            <Modal opened={opened} onClose={close} title="Authentication">
                <DeskAddModal locationId={id} />
            </Modal>
            <div style={{width: "100%", textAlign: "center"}}>
                <Title order={1} mb="md">
                    All Desks for {location?.name ?? "Unknown Location"}
                </Title>
                {userData?.userRole === 1 ? (
                    <Button onClick={open} variant="outline" style={{marginBottom: '20px'}}>
                        Add New Desk
                    </Button>
                ) : null}
            </div>
            {desks.length > 0 ? (
                <ModalsProvider>
                <MantineReactTable
                    columns={columns}
                    data={desks}
                    enableColumnActions={false}
                    enableSorting={false}
                />
                </ModalsProvider>
            ) : (
                <Text pt={"xl"}>No desks available at this location.</Text>
            )}
        </div>
    );
}
