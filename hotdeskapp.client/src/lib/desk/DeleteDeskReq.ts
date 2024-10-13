import axiosInstance from "../AxiosInstance.tsx";

async function DeleteDeskReq(id: string){
    try {
        const response = await axiosInstance.delete('admin/desk/'+id);
        return response.data;
    } catch (error) {
        throw error;
    }
}
export default DeleteDeskReq;
