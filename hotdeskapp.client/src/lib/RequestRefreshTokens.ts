import axiosInstance from "./AxiosInstance.tsx";

async function RequestRefreshTokens() {
    const response = await axiosInstance.post('Tokens/refresh');
    return response.data;
}

export default RequestRefreshTokens;

