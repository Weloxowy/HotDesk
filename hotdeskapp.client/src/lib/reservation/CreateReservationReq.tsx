import axiosInstance from "../AxiosInstance.tsx";
import ReservationsPublic from "../interfaces.ts";


async function CreateReservationReq(data : ReservationsPublic){
    try {
        const response = await axiosInstance.post('reservation/', {
            userId: data.userId,
            deskId: data.deskId,
            startDate: data.startDate,
            endDate: data.endDate
        });
        console.log(response.data);
        return response.data;
    } catch (error) {
        throw error;
    }
}
export default CreateReservationReq;
