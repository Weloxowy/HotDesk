import {Button, Image, Paper, Text, Title} from "@mantine/core";
import LocationItem from "../../lib/interfaces.ts";

const LocationPaper = ({ loc }: { loc: LocationItem }) => {
    const handleClick = () => {
        window.location.href = "location/" + loc.id;
    };

    return (
        <Paper shadow="xl" p={"xl"} withBorder>
            <Image src={/*loc.coverImgPath*/"../../../public/offices/"+Math.floor(Math.random() * (5 - 1 + 1) + 1)+".jpg"} alt={loc.name} height={360} fit="cover" />
            <Title order={2} mt="md">
                {loc.name}
            </Title>
            <Text>{loc.description}</Text>
            <Button mt="md" onClick={handleClick}>
                Wejdź
            </Button>
        </Paper>
    );
};

export default LocationPaper;
