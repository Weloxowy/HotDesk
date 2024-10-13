import axiosInstance from "./AxiosInstance.tsx";

async function RequestRefreshTokens() {
    const response = await axiosInstance.post('auth/refresh');
    return response.data;
}

export default RequestRefreshTokens;

