import axiosInstance from "../AxiosInstance.tsx";

async function LoginReq(email: string, password: string) {

    try {
        const response = await axiosInstance.post('auth/login', {
            email: email, password: password,
        });
        return response.data;
    } catch (error) {
        throw error;
    }
}

export default LoginReq;

