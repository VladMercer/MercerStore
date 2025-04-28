import React from 'react';
import {useReviews} from '../hooks/useReviews';

const TotalReviewsInfoComponent = () => {
    const {totalReviews, pageNumber, pageSize} = useReviews();

    return (
        <div className="total-products">
            <h3>
                Показано {(pageNumber - 1) * pageSize + 1} - {Math.min(pageNumber * pageSize, totalReviews)} из {totalReviews}
            </h3>

        </div>
    );
};

export default TotalReviewsInfoComponent;