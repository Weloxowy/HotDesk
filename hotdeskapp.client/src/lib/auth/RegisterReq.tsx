import axiosInstance from "../AxiosInstance.tsx";

async function RegisterReq(name: string, surname: string, email: string, password: string) {

    try {
        const response = await axiosInstance.post('auth/register', {
            name: name, surname: surname, email: email, password: password,
        });
        return response.data;
    } catch (error) {
        throw error;
    }
}

export default RegisterReq;

