import React from "react";
import { AreaChart, Area, XAxis, YAxis, Tooltip, ResponsiveContainer } from "recharts";
import { useMetrics } from "../hooks/useMetrics";

const InvoicesChartComponent = () => {
    const { invoices, isLoaded } = useMetrics();

    if (!isLoaded) return <div>Загрузка...</div>;

    const data = [
        { name: "День", value: invoices.totalInvoices.daily },
        { name: "Неделя", value: invoices.totalInvoices.weekly },
        { name: "Месяц", value: invoices.totalInvoices.monthly },
        { name: "Год", value: invoices.totalInvoices.yearly },
    ];

    return (
        <div style={{ width: "400px", height: "300px" }}>
            <h3>Счета</h3>
            <ResponsiveContainer width="100%" height="90%">
                <AreaChart data={data}>
                    <XAxis dataKey="name" />
                    <YAxis />
                    <Tooltip />
                    <Area type="monotone" dataKey="value" stroke="#8884d8" fill="#8884d8" />
                </AreaChart>
            </ResponsiveContainer>
        </div>
    );
};

export default InvoicesChartComponent;