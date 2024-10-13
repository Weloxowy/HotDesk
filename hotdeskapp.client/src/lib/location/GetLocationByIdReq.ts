import axiosInstance from "../AxiosInstance.tsx";

async function GetLocationByIdReq(id: string) {

    try {
        const response = await axiosInstance.get('location/'+id);
        return response.data;
    } catch (error) {
        throw error;
    }
}

export default GetLocationByIdReq;

