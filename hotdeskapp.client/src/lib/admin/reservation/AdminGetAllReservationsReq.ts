import axiosInstance from "../../AxiosInstance.tsx";


async function AdminGetAllReservationsReq() {

    try {
        const response = await axiosInstance.get('admin/reservation/all');
        return response.data;
    } catch (error) {
        throw error;
    }
}

export default AdminGetAllReservationsReq;

