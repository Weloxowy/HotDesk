import axiosInstance from "../AxiosInstance.tsx";

async function CreateDeskReq(name:string, description: string, locationId: string){
    try {
        const response = await axiosInstance.post('admin/desk/', {
            id: "3fa85f64-5717-4562-b3fc-2c963f66afa6",
            locationId: locationId,
            name: name,
            description: description,
            isMaintnance: false
        });
        return response.data;
    } catch (error) {
        throw error;
    }
}
export default CreateDeskReq;
