import axiosInstance from "../AxiosInstance.tsx";

async function GetUserReq() {

    try {
        const response = await axiosInstance.get('user/user');
        console.log(response.data);
        return response.data;
    } catch (error) {
        throw error;
    }
}

export default GetUserReq;

