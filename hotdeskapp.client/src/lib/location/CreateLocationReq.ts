import axiosInstance from "../AxiosInstance.tsx";

async function CreateLocationReq(name:string, description: string){
    try {
        const response = await axiosInstance.post('admin/location/', {
            id: "3fa85f64-5717-4562-b3fc-2c963f66afa6",
            name: name,
            description: description,
            coverImgPath: "string"
        });
        return response.data;
    } catch (error) {
        throw error;
    }
}
export default CreateLocationReq;
