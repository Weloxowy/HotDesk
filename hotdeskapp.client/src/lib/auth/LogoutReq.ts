import axiosInstance from "../AxiosInstance.tsx";

async function LogoutReq() {
    try {
        const response = await axiosInstance.get('auth/logout');
        return response.data;
    } catch (error) {
        throw error;
    }
}

export default LogoutReq;

