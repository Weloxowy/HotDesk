import {useEffect, useState} from "react";
import {Loader, Title} from "@mantine/core";
import GetAllLocationsReq from "../../lib/location/GetAllLocationsReq.ts";
import LocationPaper from "./LocationPaper.tsx";
import LocationItem from "../../lib/interfaces.ts";


export default function AllLocations() {
    const [locations, setLocations] = useState<LocationItem[]>([]);
    const [loading, setLoading] = useState<boolean>(true);
    const [error, setError] = useState<string | null>(null);

    useEffect(() => {
        const fetchLocations = async () => {
            try {
                const data = await GetAllLocationsReq();
                setLocations(data);
            } catch (err) {
                setError("Failed to fetch locations");
            } finally {
                setLoading(false);
            }
        };

        fetchLocations();
    }, []);

    if (loading) return <Loader/>;
    if (error) return <div>{error}</div>;

    return (
        <>
            <div style={{
                marginTop: "4rem",
                display: "flex",
                flexDirection: "column",
                alignItems: "center",
                justifyContent: "center"
            }}>
                <Title order={1} mb="md" style={{textAlign: "center"}}>
                    All locations
                </Title>
            </div>
            <div>
                {locations.map((loc) => (
                    <LocationPaper key={loc.id} loc={loc}/>
                ))}
            </div>
        </>
    );
}
