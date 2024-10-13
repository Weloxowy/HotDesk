import axiosInstance from "../AxiosInstance.tsx";

async function GetDeskAvailabilityReq(deskId : string, month: number, year: number) {

    try {
        const response = await axiosInstance.get('desk/desk-availability/'+deskId+'?month='+month+'&year='+year);
        console.log(response.data);
        return response.data;
    } catch (error) {
        throw error;
    }
}

export default GetDeskAvailabilityReq;
