using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ballistics : MonoBehaviour
{
    static public Vector2 Ballistic(Vector2 target, float velocity, float gravity, bool is_upper)
    {

        Vector2 result = Vector2.zero;
        float x = target.x;
        float y = target.y;
        float expect_time;
        if (((gravity * y + velocity * velocity) * (gravity * y + velocity * velocity) - (x * x + y * y) * gravity * gravity) >= 0)
        {
            expect_time = Mathf.Sqrt((2 * (gravity * y + velocity * velocity) + (is_upper ? 1 : -1) * 2 * Mathf.Sqrt((gravity * y + velocity * velocity) * (gravity * y + velocity * velocity) - (x * x + y * y) * gravity * gravity)) / (gravity * gravity));
            result = new Vector2(x / (velocity * expect_time), 0);
            result.y = Mathf.Sqrt(1 - result.x * result.x);
        }
        else
        {

            result = new Vector2(Mathf.Sqrt(0.5f) * (x > 0 ? 1 : -1), Mathf.Sqrt(0.5f));
        }

        result *= velocity;

        result = new Vector2(result.x == float.NaN ? 0 : result.x, result.y == float.NaN ? 0 : result.y);

        return result;
    }

    static public Vector2 Ballistic(Vector2 target, float velocity, float gravity) {

        return Ballistic(target, velocity, gravity, GameController.Level < 2 ? true : false);
    }
}
