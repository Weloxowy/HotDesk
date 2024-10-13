import axiosInstance from "../AxiosInstance.tsx";

async function GetAllLocationsReq() {

    try {
        const response = await axiosInstance.get('location/all');
        return response.data;
    } catch (error) {
        throw error;
    }
}

export default GetAllLocationsReq;

