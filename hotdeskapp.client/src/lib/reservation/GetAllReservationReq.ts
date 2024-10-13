import axiosInstance from "../AxiosInstance.tsx";

async function GetAllReservationReq() {

    try {
        const response = await axiosInstance.get('reservation/all');
        return response.data;
    } catch (error) {
        throw error;
    }
}

export default GetAllReservationReq;

