import axiosInstance from "../AxiosInstance.tsx";

async function GetAllLocationsReq(locationId : string) {

    try {
        const response = await axiosInstance.get('desk/all/location/'+locationId);
        return response.data;
    } catch (error) {
        throw error;
    }
}

export default GetAllLocationsReq;

