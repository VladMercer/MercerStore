import React from "react";
import { BarChart, Bar, XAxis, YAxis, Tooltip, ResponsiveContainer } from "recharts";
import { useMetrics } from "../hooks/useMetrics";
import useFetchMetrics from "../hooks/useFetchMetrics";


const SalesChartComponent = () => {
    const { sales, isLoaded } = useMetrics();
    useFetchMetrics();
    if (!isLoaded) return <div>Загрузка...</div>;

    const data = [
        { name: "День", value: sales.daily },
        { name: "Неделя", value: sales.weekly },
        { name: "Месяц", value: sales.monthly },
        { name: "Год", value: sales.yearly },
    ];

    return (
        <div style={{ width: "400px", height: "300px" }}>
            <h3>Продажи</h3>
            <ResponsiveContainer width="100%" height="90%">
                <BarChart data={data}>
                    <XAxis dataKey="name" />
                    <YAxis />
                    <Tooltip />
                    <Bar dataKey="value" fill="#8884d8" />
                </BarChart>
            </ResponsiveContainer>
        </div>
    );
};

export default SalesChartComponent;
