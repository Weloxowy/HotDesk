import {Button, Group, Image, Paper, Text, Title} from "@mantine/core";
import LocationItem from "../../lib/interfaces.ts";
import DeleteLocationReq from "../../lib/location/DeleteLocationReq.ts";

const LocationPaper = ({ loc, isAdmin }: { loc: LocationItem, isAdmin : boolean }) => {
    const handleClick = () => {
        window.location.href = "location/" + loc.id;
    };

     const handleDelete = async () => {
        try{
            await DeleteLocationReq(loc.id);
            alert("Location deleted");
            window.location.reload();
        }
        catch (error) {
           alert(error.response.data);
        }

    };

    return (
        <Paper shadow="xl" p={"xl"} withBorder>
            <Image src={/*loc.coverImgPath*/"../../../public/offices/"+Math.floor(Math.random() * (5 - 1 + 1) + 1)+".jpg"} alt={loc.name} height={360} fit="cover" />
            <Title order={2} mt="md">
                {loc.name}
            </Title>
            <Text>{loc.description}</Text>
            <Group style={{display:"flex", width:"100%", alignItems:"stretch"}}>
                <Button mt="md" onClick={handleClick}>
                    Go to location
                </Button>
                {isAdmin ? (
                    <Button mt="md" variant={"outline"} color={"red"} onClick={handleDelete}>Delete location</Button>
                ): (<></>)
                }
            </Group>
        </Paper>
    );
};

export default LocationPaper;
