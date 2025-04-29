import React from "react";
import {Cell, Pie, PieChart, ResponsiveContainer, Tooltip} from "recharts";
import {useMetrics} from "../hooks/useMetrics";

const ReviewsChartComponent = () => {
    const {reviews, isLoaded} = useMetrics();

    if (!isLoaded) return <div>Загрузка...</div>;

    const data = [
        {name: "Всего", value: reviews.total},
        {name: "Новые за месяц", value: reviews.newReviews.monthly},
    ];

    const COLORS = ["#0088FE", "#00C49F"];

    return (
        <div style={{width: "400px", height: "300px"}}>
            <h3>Отзывы</h3>
            <ResponsiveContainer width="100%" height="90%">
                <PieChart>
                    <Pie data={data} cx="50%" cy="50%" outerRadius={80} fill="#8884d8" dataKey="value">
                        {data.map((_, index) => (
                            <Cell key={`cell-${index}`} fill={COLORS[index % COLORS.length]}/>
                        ))}
                    </Pie>
                    <Tooltip/>
                </PieChart>
            </ResponsiveContainer>
        </div>
    );
};

export default ReviewsChartComponent;