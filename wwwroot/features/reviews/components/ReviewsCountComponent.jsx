import React from 'react';
import { useReviews } from '../hooks/useReviews';

const ReviewsCountComponent = () => {
    const { countReviews } = useReviews();

    const getReviewText = (count) => {
        if (count % 10 === 1 && count % 100 !== 11) return 'оценка';
        if (count % 10 >= 2 && count % 10 <= 4 && (count % 100 < 10 || count % 100 >= 20)) return 'оценки';
        return 'оценок';
    };
    return (
        <div className="count-reviews-text">
            {countReviews !== 0 ? (
                <h5>
                    <span className="count-reviews">
                        {countReviews} {getReviewText(countReviews)}
                    </span>
                    </h5>

                    ) : (
                    <h5>
                        <span className="no-reviews">Нет оценок</span>
                        </h5>
               
            )}
                    </div>
                    );
}

                    export default ReviewsCountComponent;