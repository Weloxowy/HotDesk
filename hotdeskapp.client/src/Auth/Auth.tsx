import {Anchor, Button, Group, Image, Paper, PasswordInput, rem, Text, TextInput,} from '@mantine/core';
import classes from './Auth.module.css';
import {useToggle} from '@mantine/hooks';
import {useForm} from '@mantine/form';
import loginReq from '../lib/auth/LoginReq.tsx';
import registerReq from "../lib/auth/RegisterReq.tsx";

export default function Auth() {
    const [type, toggle] = useToggle(['login', 'register']);

    const form = useForm({
        initialValues: {
            email: '',
            name: '',
            surname: '',
            password: '',
        },
        validate: {
            name: (val) => (type === 'register' && val.length < 2) ? 'Name should be longer than 1 character' : null,
            surname: (val) => (type === 'register' && val.length < 2) ? 'Surname should be longer than 1 character' : null,
            email: (val) =>
                /^\S+@\S+$/.test(val) ? null : 'Invalid email',
            password: (val) =>
                /^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)[a-zA-Z\d]{8,}$/.test(val) ? null : 'Password should include at minimum 8 characters, at least one uppercase letter, one lowercase letter and one number',
        },
    });

    const handleLogin = async (values: { email: string; password: string }) => {
        try {
            console.log('Login submitted');
            await loginReq(values.email, values.password);
            window.location.href = '/locations';
        } catch (error) {
            console.error('Error in handleLogin:', error);
            switch (error.status) {
                case 401:
                    form.setErrors({password: 'Podano nieprawidłowe dane uwierzytelniające'});
                    break;
                case 404:
                    form.setErrors({password: 'Podany użytkownik nie istnieje'});
                    break;
                case 500:
                    form.setErrors({password: 'Wystąpił wewnętrzny błąd serwera. Spróbuj ponownie później'});
                    break;
                default:
                    form.setErrors({password: 'Wystąpił nieznany błąd'});
                    break;
            }
        }
    };

    const handleRegisterForm = async (values: { name: string; surname: string; email: string; password: string }) => {
        try {

            await registerReq(values.name, values.surname, values.email, values.password);
            window.location.href = '/locations';
        } catch (error: any) {
            console.error('Error in handleRegisterForm:', error);
            switch (error.status) {
                case 401:
                    form.setErrors({password: 'Podano nieprawidłowe dane uwierzytelniające'});
                    break;
                case 404:
                    form.setErrors({password: 'Podany użytkownik nie istnieje'});
                    break;
                case 500:
                    form.setErrors({password: 'Wystąpił wewnętrzny błąd serwera. Spróbuj ponownie później'});
                    break;
                default:
                    form.setErrors({password: 'Wystąpił nieznany błąd'});
                    break;
            }
        }
    };

    const handleSubmit = async (values: any) => {
        console.log('Submitting form:', type, values);
        if (type === 'register') {
            await handleRegisterForm(values);
        } else {
            await handleLogin(values);
        }
    };


    return (
        <div className={classes.wrapper}>
            <Paper className={classes.form} radius={0} p={30}>
                <div
                    style={{
                        alignItems: 'center',
                        justifyContent: 'center',
                        display: 'flex',
                        flex: '1 1',
                    }}
                >
                    <Image src='../../public/whiteLogo.svg' w={rem(300)}/>
                </div>

                <form onSubmit={form.onSubmit(handleSubmit)}>
                    {type === 'register' && (
                        <Group grow>
                            <TextInput
                                label='Name'
                                value={form.values.name}
                                key={form.key('name')}
                                {...form.getInputProps('name')}
                                placeholder='Artur'
                                size='md'
                                mt='md'
                            />
                            <TextInput
                                label='Surname'
                                value={form.values.surname}
                                key={form.key('surname')}
                                {...form.getInputProps('surname')}
                                placeholder='Wiech'
                                size='md'
                                mt='md'
                            />
                        </Group>
                    )}

                    <TextInput
                        label='Email address'
                        value={form.values.email}
                        key={form.key('email')}
                        {...form.getInputProps('email')}
                        placeholder='hello@gmail.com'
                        size='md'
                        mt='md'
                    />
                    <PasswordInput
                        label='Password'
                        value={form.values.password}
                        key={form.key('password')}
                        {...form.getInputProps('password')}
                        placeholder='Your password'
                        mt='md'
                        size='md'
                    />
                    <Button type='submit' fullWidth mt='xl' size='md'>
                        {type === 'register' ? 'Register' : 'Login'}
                    </Button>
                </form>
                {type === 'register' ? (
                    <Text ta='center' mt='md'>
                        Already have an account?{' '}
                        <Anchor<'a'> href='#' fw={700} onClick={() => toggle()}>
                            Login
                        </Anchor>
                    </Text>
                ) : (
                    <Text ta='center' mt='md'>
                        Don't have an account?{' '}
                        <Anchor<'a'> href='#' fw={700} onClick={() => toggle()}>
                            Register
                        </Anchor>
                    </Text>
                )}
            </Paper>
        </div>
    );
}
