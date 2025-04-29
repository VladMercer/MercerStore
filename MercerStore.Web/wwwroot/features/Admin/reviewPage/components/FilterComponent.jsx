import React from "react";
import {useDispatch} from "react-redux";
import {useReviews} from "../hooks/useReviews";
import {setFilter} from "../redux/reviewPageSlice";

const FilterComponent = () => {
    const dispatch = useDispatch();
    const {statusFilter} = useReviews();

    const handleFilterChange = (event) => {
        dispatch(setFilter(event.target.value));
    };

    return (
        <div className="input-group mb-3">
            <span className="input-group-text">Фильтр:</span>
            <select className="form-select" value={statusFilter} onChange={handleFilterChange}>
                <option value="All">Все отзывы</option>
                <option value="Value1">Оценка 1</option>
                <option value="Value2">Оценка 2</option>
                <option value="Value3">Оценка 3</option>
                <option value="Value4">Оценка 4</option>
                <option value="Value5">Оценка 5</option>
                <option value="HasReviewText">С текстом</option>
                <option value="NoReviewText">Без текста</option>
                <option value="CreateDate">По дате создания</option>
                <option value="EditDate">По дате редактирования</option>
            </select>
        </div>
    );
};

export default FilterComponent;