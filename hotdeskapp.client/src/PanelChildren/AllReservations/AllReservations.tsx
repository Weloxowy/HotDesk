import { useEffect, useMemo, useState } from 'react';
import {
    MantineReactTable,
    useMantineReactTable,
    type MRT_ColumnDef, MRT_Row,
} from 'mantine-react-table';
import ReservationsPublic from "../../lib/interfaces.ts";
import { ModalsProvider, modals } from '@mantine/modals';
import { Text } from '@mantine/core';
import { IconTrash } from '@tabler/icons-react';
import {Button, Group} from "@mantine/core";
import DeleteReservationReq from "../../lib/reservation/DeleteReservationReq.ts";
import AdminGetAllReservationsReq from "../../lib/admin/reservation/AdminGetAllReservationsReq.ts";

export default function AllReservations() {
    const [data, setData] = useState<ReservationsPublic[]>([]);
    const [loading, setLoading] = useState<boolean>(true);
    const [error, setError] = useState<string | null>(null);

    useEffect(() => {
        const fetchData = async () => {
            try {
                const userData = await AdminGetAllReservationsReq();
                setData(userData);
            } catch{
                setError('Failed to fetch data');
            } finally {
                setLoading(false);
            }
        };

        fetchData();
    }, []);

    const isReservationCancelable = (startDate: string) => {
        const reservationStart = new Date(startDate);
        const now = new Date();
        const hoursDifference = (reservationStart.getTime() - now.getTime()) / (1000 * 3600);
        return hoursDifference >= 24;
    };

    const openDeleteConfirmModal = (row: MRT_Row<ReservationsPublic>) =>
        modals.openConfirmModal({
            title: 'Are you sure you want to delete this reservation?',
            children: (
                <Text>
                    Are you sure you want to delete reservation for {row.original.deskName}? This action cannot be undone.
                </Text>
            ),
            labels: { confirm: 'Delete', cancel: 'Cancel' },
            confirmProps: { color: 'red' },
            onConfirm: async () => {
                try {
                    await DeleteReservationReq(row.original.id);
                    setData((prevData) => prevData.filter((reservation) => reservation.id !== row.original.id));
                } catch{
                    setError('Failed to delete reservation');
                }
            },
        });

    const columns = useMemo<MRT_ColumnDef<ReservationsPublic>[]>(() => [
        {
            accessorKey: 'id',
            header: 'Reservation ID',
        },
        {
            header: 'User',
            accessorKey: `name`,
            accessorFn: (row) => `${row.name} ${row.surname}`
        },
        {
            accessorKey: 'locationName',
            header: 'Location',
        },
        {
            accessorKey: 'deskName',
            header: 'Desk',
        },
        {
            accessorKey: 'startDate',
            header: 'Start Date',
        },
        {
            accessorKey: 'endDate',
            header: 'End Date',
        },
        {
            accessorKey: 'actions',
            header: 'Actions',
            Cell: ({ row }) => (
                <Group m={"xs"}>
                    <Button
                        variant="subtle"
                        color="red"
                        leftSection={<IconTrash />}
                        onClick={() => openDeleteConfirmModal(row)}
                    >
                        Delete
                    </Button>
                </Group>
            ),
        },
    ], []);

    const table = useMantineReactTable({
        columns,
        data,
        state: { isLoading: loading },
    });

    if (error) {
        return <div>{error}</div>;
    }

    return (
        <ModalsProvider>
            <MantineReactTable table={table} />
        </ModalsProvider>
    );
}
