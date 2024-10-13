import {Button, Loader, Text, Title} from "@mantine/core";
import Desk from "../../lib/interfaces.ts";
import { MantineReactTable, MRT_ColumnDef } from "mantine-react-table";
import { useEffect, useMemo, useState } from "react";
import GetAllDesksByLocationReq from "../../lib/desk/GetAllDesksByLocationReq.ts";
import { Link, useParams } from "react-router-dom";
import LocationItem from "../../lib/interfaces.ts";
import getLocationByIdReq from "../../lib/location/GetLocationByIdReq.ts";

export default function LocationDesks() {
    const { id } = useParams();
    const [desks, setDesks] = useState<Desk[]>([]);
    const [location, setLocation] = useState<LocationItem>();
    const [loading, setLoading] = useState<boolean>(true);
    const [error, setError] = useState<string | null>(null);

    useEffect(() => {
        const fetchDesks = async () => {
            try {
                const dataDesks = await GetAllDesksByLocationReq(id);
                const dataLocation = await getLocationByIdReq(id);
                setDesks(dataDesks);
                setLocation(dataLocation);
            } catch (err) {
                setError("Failed to fetch desks");
            } finally {
                setLoading(false);
            }
        };

        fetchDesks();
    }, [id]);


    const columns = useMemo<MRT_ColumnDef<Desk>[]>(
        () => [
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
                    <Button component={Link} to={`/desk/${row.original.id}`} size="sm">
                        Go to Desk
                    </Button>
                ),
            },
        ],
        []
    );

    if (loading) return <Loader />;
    if (error) return <div>{error}</div>;

    return (
        <div style={{paddingTop:"4rem"}}>
            {location && (
                <>
                <Title order={1} mb="md">
                    All Desks for {location.name}
                </Title>
                <MantineReactTable
                columns={columns}
            data={desks}
            enableColumnActions={false}
            enableSorting={false}
        />
                </>
            )}
        </div>
    );
}
