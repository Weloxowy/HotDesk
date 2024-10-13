import axiosInstance from "../AxiosInstance.tsx";

async function DeleteLocationReq(id:string){
    try {
        const response = await axiosInstance.delete('admin/location/'+id);
        return response.data;
    } catch (error) {
        throw error;
    }
}
export default DeleteLocationReq;
