import React, { useState } from 'react';
import { Button, TextInput, Group, Text } from '@mantine/core';
import createLocationReq from "../../lib/location/CreateLocationReq.ts";

const LocationAddModal = () => {
    const [name, setName] = useState('');
    const [description, setDescription] = useState('');
    const [errors, setErrors] = useState<{ name?: string; description?: string }>({});

    const handleSubmit = () => {
        const validationErrors: { name?: string; description?: string } = {};
        if (!name) validationErrors.name = 'Name is required';
        if (!description) validationErrors.description = 'Description is required';

        if (Object.keys(validationErrors).length > 0) {
            setErrors(validationErrors);
            return;
        }
        try {
            createLocationReq(name, description);
            setName('');
            setDescription('');
            alert("Location created successfully");
            window.location.reload();
        } catch (error: any) {
            if (error.response && error.response.errors) {
                const fieldErrors = error.response.errors;
                setErrors({
                    name: fieldErrors.name || undefined,
                    description: fieldErrors.description || undefined,
                });
            }
        }
    };

    return (
        <form onSubmit={(e) => e.preventDefault()}>
            <Text size="xl" fw={500}>
                Name
            </Text>
            <TextInput
                value={name}
                placeholder="Enter name"
                onChange={(e) => {
                    setName(e.target.value);
                    setErrors((prevErrors) => ({ ...prevErrors, name: undefined }));
                }}
                error={errors.name}
            />
            <Text size="xl" fw={500} mt="lg">
                Description
            </Text>
            <TextInput
                value={description}
                placeholder="Enter description"
                onChange={(e) => {
                    setDescription(e.target.value);
                    setErrors((prevErrors) => ({ ...prevErrors, description: undefined }));
                }}
                error={errors.description}
            />
            <Group position="right" mt="xl">
                <Button onClick={handleSubmit}>Submit</Button>
            </Group>
        </form>
    );
};

export default LocationAddModal;
