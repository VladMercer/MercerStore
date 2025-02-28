import React from "react";
import { LineChart, Line, XAxis, YAxis, Tooltip, ResponsiveContainer } from "recharts";
import { useMetrics } from "../hooks/useMetrics";

const UsersChartComponent = () => {
    const { users, isLoaded } = useMetrics();

    if (!isLoaded) return <div>Загрузка...</div>;

    const data = [
        { name: "День", value: users.newUsers.daily },
        { name: "Неделя", value: users.newUsers.weekly },
        { name: "Месяц", value: users.newUsers.monthly },
        { name: "Год", value: users.newUsers.yearly },
    ];

    return (
        <div style={{ width: "400px", height: "300px" }}>
            <h3>Новые пользователи</h3>
            <ResponsiveContainer width="100%" height="90%">
                <LineChart data={data}>
                    <XAxis dataKey="name" />
                    <YAxis />
                    <Tooltip />
                    <Line type="monotone" dataKey="value" stroke="#82ca9d" />
                </LineChart>
            </ResponsiveContainer>
        </div>
    );
};

export default UsersChartComponent;