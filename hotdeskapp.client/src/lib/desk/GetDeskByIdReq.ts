import axiosInstance from "../AxiosInstance.tsx";

async function GetDeskById(deskId : string) {

    try {
        const response = await axiosInstance.get('desk/'+deskId);
        console.log(response.data);
        return response.data;
    } catch (error) {
        throw error;
    }
}

export default GetDeskById;
