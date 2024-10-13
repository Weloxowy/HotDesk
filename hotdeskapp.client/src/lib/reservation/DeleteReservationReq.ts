import axiosInstance from "../AxiosInstance.tsx";

async function DeleteReservationReq(id: string) {

    try {
        const response = await axiosInstance.delete('reservation/'+id);
        return response.data;
    } catch (error) {
        throw error;
    }
}

export default DeleteReservationReq;

