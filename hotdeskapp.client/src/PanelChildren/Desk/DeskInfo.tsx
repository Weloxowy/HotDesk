import {Button, MantineThemeProvider, Paper, rem, Text, Title} from "@mantine/core";
import {DatePicker} from "@mantine/dates";
import { useEffect, useState } from "react";
import GetDeskAvailabilityReq from "../../lib/desk/GetDeskAvailabilityReq";
import {useParams} from "react-router-dom";
import CreateReservationReq from "../../lib/reservation/CreateReservationReq.tsx";
import ReservationsPublic from "../../lib/interfaces.ts";
import GetUserReq from "../../lib/user/GetUserReq.ts";
import getDeskByIdReq from "../../lib/desk/GetDeskByIdReq.ts";
import Desk from "../../lib/interfaces.ts";

export default function DeskInfo() {
    const { id } = useParams();
    const [availability, setAvailability] = useState<{ [key: string]: boolean[] }>({});
    const [currentMonth, setCurrentMonth] = useState<Date>(new Date());
    const [loading, setLoading] = useState<boolean>(false);
    const [selectedRange, setSelectedRange] = useState<[Date | null, Date | null]>([null, null]);
    const [desk, setDesk] = useState<Desk>([]);

    useEffect(() => {
        const fetchLocations = async () => {
                const data = await getDeskByIdReq(id);
                setDesk(data);
                setLoading(false);
        };

        fetchLocations();
    }, []);

    const handleReservation = async () => {
        try {
            const user = await GetUserReq();
            if (!user || !user.id) {
                throw new Error("User not found or invalid user data.");
            }

            if (!selectedRange[0] || !selectedRange[1]) {
                alert("Please select a valid date range.");
                return;
            }

            const startDate = new Date(selectedRange[0]);
            startDate.setUTCHours(12, 0, 0, 0);
            startDate.setUTCDate(startDate.getUTCDate() + 1);

            const endDate = new Date(selectedRange[1]);
            endDate.setUTCHours(12, 0, 0, 0);
            endDate.setUTCDate(endDate.getUTCDate() + 1);

            const req: ReservationsPublic = {
                startDate: startDate.toISOString(),
                endDate: endDate.toISOString(),
                deskId: id?.toString(),
                userId: user.id
            };

            await CreateReservationReq(req);
            alert("Reservation created successfully.");
            window.location.reload();

        } catch (error) {
            console.error("Error during reservation:", error);
            if (error.message.includes("User not found")) {
                alert("Unable to fetch user data. Please try again.");
            } else {
                alert("Failed to create reservation. Please check your selected dates and try again.");
            }
        }
    };

    const fetchAvailability = async (year: number, month: number) => {
        try {
            setLoading(true);
            const result = await GetDeskAvailabilityReq(id, month, year);
            setAvailability((prev) => ({
                ...prev,
                [`${year}-${month}`]: result,
            }));
        } catch (error) {
            alert("Error fetching availability:"+ error);
        } finally {
            setLoading(false);
        }
    };


    useEffect(() => {
        const year = currentMonth.getFullYear();
        const month = currentMonth.getMonth() + 1;

        if (!availability[`${year}-${month}`]) {
            fetchAvailability(year, month);
        }
    }, [currentMonth]);

    const shouldDisableDate = (date: Date) => {
        const year = date.getFullYear();
        const month = date.getMonth() + 1;
        const day = date.getDate() - 1;
        const monthAvailability = availability[`${year}-${month}`];

        return monthAvailability ? !monthAvailability[day] : true;
    };

    const areAllDatesAvailable = (start: Date, end: Date) => {
        let currentDate = new Date(start);
        while (currentDate <= end) {
            if (shouldDisableDate(currentDate)) {
                return false;
            }
            currentDate.setDate(currentDate.getDate() + 1);
        }
        return true;
    };

    const theme = MantineThemeProvider;
    const numberOfComponents = 7;
    const [width, setWidth] = useState(0);

    useEffect(() => {
        function handleResize() {
            setWidth((window.innerWidth * 0.55) / numberOfComponents);
        }
        handleResize();
        window.addEventListener('resize', handleResize);
        return () => {
            window.removeEventListener('resize', handleResize);
        };
    }, []);

    const handleRangeChange = (range: [Date | null, Date | null]) => {
        const [start, end] = range;
        if (start && end) {
            const dayCount = Math.ceil((end.getTime() - start.getTime()) / (1000 * 3600 * 24)) + 1;

            if (dayCount > 7) {
                alert("You can select a maximum of 7 days.");
                return;
            }

            if (!areAllDatesAvailable(start, end)) {
                alert("One or more selected days are not available.");
                return;
            }
        }

        setSelectedRange(range);
    };

    return (
        <div style={{marginTop: "4rem", display: "flex", flexDirection: "column", alignItems: "center"}}>
            {
                desk && (
                    <section style={{textAlign: "center", marginBottom: "2rem"}}>
                        <Title order={1}>{desk.name}</Title>
                        <Text c={"gray"}>{desk.description}</Text>
                    </section>
                )
            }
            <Paper shadow="lg" style={{
                padding: "2rem",
                maxWidth: "100%",
                width: "100%",
                display: "flex",
                flexDirection: "column",
                alignItems: "center"
            }}>
                <Title order={2} style={{marginBottom: "1.5rem", textAlign: "center"}}>Select dates</Title>

                {loading && <Text>Loading availability...</Text>}

                <div style={{width: "100%", display: "flex", justifyContent: "center"}}>
                    <DatePicker
                        type="range"
                        size="xl"
                        allowSingleDateInRange
                        value={selectedRange}
                        onChange={handleRangeChange}
                        month={currentMonth}
                        onMonthChange={setCurrentMonth}
                        excludeDate={shouldDisableDate}
                        allowLevelChange={false}
                        styles={(theme) => ({
                            display: 'flex',
                            justifyContent: 'center',
                            alignItems: 'center',
                            cell: {
                                border: `1px solid ${
                                    theme.colorScheme === 'dark' ? theme.colors.dark[4] : theme.colors.gray[2]
                                }`,
                            },
                            day: { borderRadius: 0, flexGrow: 1, height: rem(70), width: `${width}px`, fontSize: theme.fontSizes.lg },

                            weekday: { fontSize: theme.fontSizes.lg },
                            weekdayCell: {
                                fontSize: theme.fontSizes.xl,
                                backgroundColor:
                                    theme.colorScheme === 'dark' ? theme.colors.dark[5] : theme.colors.gray[0],
                                border: `1px solid ${
                                    theme.colorScheme === 'dark' ? theme.colors.dark[4] : theme.colors.gray[2]
                                }`,
                                height: 10,
                            }
                        })}
                    />
                </div>

                {selectedRange[0] && selectedRange[1] && (
                    <>
                        <Text style={{ marginTop: "1rem", textAlign: "center" }}>
                            Selected Range: {selectedRange[0]?.toLocaleDateString()} - {selectedRange[1]?.toLocaleDateString()}
                        </Text>
                        <Button style={{marginTop: "1rem"}} onClick={handleReservation}>Make reservation</Button>
                    </>
                )}
            </Paper>
        </div>
    );
}
