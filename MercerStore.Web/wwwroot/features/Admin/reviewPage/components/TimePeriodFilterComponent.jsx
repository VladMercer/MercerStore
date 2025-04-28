import React from "react";
import {useDispatch} from "react-redux";
import {useReviews} from "../hooks/useReviews";
import {setTimePeriodFilter} from "../redux/reviewPageSlice";

const TimePeriodFilterComponent = () => {
    const dispatch = useDispatch();
    const {timePeriodFilter} = useReviews();

    const handleFilterChange = (event) => {
        const newFilter = event.target.value;
        dispatch(setTimePeriodFilter(newFilter));
    };
    return (
        <div className="input-group mb-3">
            <span className="input-group-text">Пероид:</span>
            <select className="form-select" value={timePeriodFilter} onChange={handleFilterChange}>
                <option value="All">За все время</option>
                <option value="Day">За этот день</option>
                <option value="Week">На этой неделе</option>
                <option value="Month">В этом месяце</option>
                <option value="Quarter">В этом квартале</option>
                <option value="Year">В этом году</option>
            </select>
        </div>
    );
};

export default TimePeriodFilterComponent;